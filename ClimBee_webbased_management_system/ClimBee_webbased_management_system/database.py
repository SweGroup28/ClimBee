import sqlite3 as dbapi2

from user import User


class Database:
    def __init__(self, dbfile):
        self.dbfile = dbfile

    def add_user(self, user):
        with dbapi2.connect(self.dbfile) as connection:
            cursor = connection.cursor()
            query = "INSERT INTO USER (NAME, SCORE, BAN) VALUES (?, ?, ?)"
            cursor.execute(query, (user.user_name, user.high_score, user.ban_status))
            connection.commit()
            user_key = cursor.lastrowid
        return user_key

    def update_user(self, user_key, user):
        with dbapi2.connect(self.dbfile) as connection:
            cursor = connection.cursor()
            query = "UPDATE USER SET NAME = ?, SCORE = ?, BAN = ? WHERE (ID = ?)"
            cursor.execute(query, (user.user_name, user.high_score, user.ban_status, user_key))
            connection.commit()

    def delete_user(self, user_key):
        with dbapi2.connect(self.dbfile) as connection:
            cursor = connection.cursor()
            query = "DELETE FROM USER WHERE (ID = ?)"
            cursor.execute(query, (user_key,))
            connection.commit()

    def get_user(self, user_key):
        with dbapi2.connect(self.dbfile) as connection:
            cursor = connection.cursor()
            query = "SELECT NAME, SCORE, BAN FROM USER WHERE (ID = ?)"
            cursor.execute(query, (user_key,))
            user_name, high_score, ban_status = cursor.fetchone()
        user_ = User(user_name, high_score=high_score, ban_status=ban_status)
        return user_

    def get_users(self):
        users = []
        with dbapi2.connect(self.dbfile) as connection:
            cursor = connection.cursor()
            query = "SELECT ID, NAME, SCORE, BAN FROM USER ORDER BY ID"
            cursor.execute(query)
            for user_key, user_name, high_score, ban_status in cursor:
                users.append((user_key, User(user_name, high_score, ban_status)))
        return users