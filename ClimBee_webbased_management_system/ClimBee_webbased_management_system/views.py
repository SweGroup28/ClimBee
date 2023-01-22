from flask import render_template, current_app, abort, request, redirect, url_for, flash

from user import User

from forms import LoginForm

from admin import get_admin

from passlib.hash import pbkdf2_sha256 as hasher


from flask_login import login_user, logout_user, current_user, login_required

def login_page():
    form = LoginForm()
    if form.validate_on_submit():
        username = form.data["username"]
        admin = get_admin(username)
        if admin is not None:
            password = form.data["password"]
            if hasher.verify(password, admin.password):
                login_user(admin)
                flash("You have logged in.")
                next_page = request.args.get("next", url_for("home_page"))
                return redirect(next_page)
        flash("Invalid credentials.")
    return render_template("login.html", form=form)


def logout_page():
    logout_user()
    flash("You have logged out.")
    return redirect(url_for("home_page"))


def home_page():
    return render_template("home.html")


def users_page():
    db = current_app.config["db"]
    if request.method == "GET":
        users = db.get_users()
        return render_template("users.html", users=sorted(users))
    else:
        if not current_user.is_admin:
            abort(401)
        
        form_user_keys = request.form.getlist("user_keys")
        for form_user_key in form_user_keys:
            db.delete_user(int(form_user_key))
        return redirect(url_for("users_page"))

def user_page(user_key):
    db = current_app.config["db"]
    user = db.get_user(user_key)
    if user is None:
        abort(404)
    return render_template("user.html", user=user)

@login_required
def user_add_page():
    if not current_user.is_admin:
        abort(401)
    if request.method == "GET":
        values = {"user_name": "", "high_score": "", "ban_status": ""}
        return render_template(
            "user_edit.html",
            min_status=0,
            max_status=1,
            values=values,
        )
    else:
        form_user_name = request.form["user_name"]
        form_high_score = request.form["high_score"]
        form_ban_status = request.form["ban_status"]
        user = User(form_user_name, high_score=int(form_high_score) if form_high_score else 0, 
        ban_status=int(form_ban_status) if form_ban_status else 0)
        db = current_app.config["db"]
        user_key = db.add_user(user)
        return redirect(url_for("user_page", user_key=user_key))

@login_required
def user_edit_page(user_key):
    if request.method == "GET":
        db = current_app.config["db"]
        user = db.get_user(user_key)
        if user is None:
            abort(404)
        values = {"user_name": user.user_name, "high_score": user.high_score, "ban_status": user.ban_status}
        return render_template(
            "user_edit.html",
            min_status=0,
            max_status=1,
            values=values,
        )
    else:
        form_user_name = request.form["user_name"]
        form_high_score = request.form["high_score"]
        form_ban_status = request.form["ban_status"]
        user = User(form_user_name, high_score=int(form_high_score) if form_high_score else 0, ban_status=int(form_ban_status) if form_ban_status else 0)
        db = current_app.config["db"]
        db.update_user(user_key, user)
        return redirect(url_for("user_page", user_key=user_key))