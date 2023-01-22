from flask import Flask
from flask_login import LoginManager

from database import Database

from admin import get_admin

import views

import os

lm = LoginManager()


@lm.user_loader
def load_user(user_id):
    return get_admin(user_id)

def create_app():
    app = Flask(__name__)
    app.config.from_object("settings")

    app.add_url_rule(
        "/login", view_func=views.login_page, methods=["GET", "POST"]
    )
    app.add_url_rule("/logout", view_func=views.logout_page)

    app.add_url_rule("/", view_func=views.home_page)
    app.add_url_rule("/users", view_func=views.users_page, methods=["GET", "POST"])
    app.add_url_rule("/users/<int:user_key>", view_func=views.user_page)
    app.add_url_rule("/users/<int:user_key>/edit", view_func=views.user_edit_page,methods=["GET", "POST"],)
    app.add_url_rule("/new-user", view_func=views.user_add_page, methods=["GET", "POST"])

    lm.init_app(app)
    lm.login_view = "login_page"

    home_dir = os.path.expanduser("./")
    db = Database(os.path.join(home_dir, "users.sqlite"))
    app.config["db"] = db

    return app


if __name__ == "__main__":
    app = create_app()
    port = app.config.get("PORT", 5000)
    app.run(host="0.0.0.0", port=port)
