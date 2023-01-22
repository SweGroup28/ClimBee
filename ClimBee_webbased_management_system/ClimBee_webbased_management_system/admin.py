from flask import current_app
from flask_login import UserMixin


class Admin(UserMixin):
    def __init__(self, username, password):
        self.username = username
        self.password = password
        self.active = True
        self.is_admin = False

    def get_id(self):
        return self.username

    @property
    def is_active(self):
        return self.active


def get_admin(user_id):
    password = current_app.config["PASSWORDS"].get(user_id)
    user = Admin(user_id, password) if password else None
    if user is not None:
        user.is_admin = user.username in current_app.config["ADMIN_USERS"]
    return user