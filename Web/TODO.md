# Workflow
## Creating New Account/Logging In
1. Guest (Unregistered User) launches app.
2. Guest enters a username and clicks _Submit_ button.
3. _UnityWebRequest_ sends the username data via _Put_ method to the specified Google Cloud Functions __URL__ as HTTP request.
4. An HTTP trigger occurs at Google Cloud _get_request_ Function.
5. The function triggers an algorithm to access a database file in cloud, search and authenticate a username if it exists. If not, a new entry is created in the database and initialized with the username.
6. An HTTP request to let the app accept the user to Main Manu is sent to _UnityWebRequest_.
## New High Score
1. The Level info is stored as _High Score_ integer value.
2. Once the level is __succesfully__ completed, the new high score is updated in the local database containing player data.
3. Then _UnityWebRequest_ sends the player data through an HTTP request via _Put_ method to the specified Google Cloud Functions __URL__ as HTTP request.
4. An HTTP trigger occurs at Google Cloud _get_high_score_ Function.
5. The function triggers an algorithm to access a database file in cloud, search and update the _High Score_ column. And finally reinserting the user according to the sort order of _High Score_.
