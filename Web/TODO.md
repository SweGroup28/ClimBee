# Workflow
## Creating New Account
1. Guest (Unregistered User) launches app.
2. Guest enters a username and clicks _Submit_ button.
3. _UnityWebRequest_ sends the username data via _Put_ method to the specified Google Cloud Functions __URL__ as HTTP request.
4. An HTTP trigger occurs at Google Cloud _GetRequest_ Function.
5. The function triggers an algorithm to access a database file in cloud, search and authenticate a username if it exists. If not, a new entry is created in the database and initialized with the username.
6. An HTTP request to let the app accept the user to Main Manu is sent to _UnityWebRequest_.
## Logging in
1. 