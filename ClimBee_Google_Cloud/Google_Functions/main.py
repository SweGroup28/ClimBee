import functions_framework
from google.cloud import bigquery

@functions_framework.http
def get_request(request):
    """HTTP Cloud Function.
    Args:
        request (flask.Request): The request object.
        <https://flask.palletsprojects.com/en/1.1.x/api/#incoming-request-data>
    Returns:
        The response text, or any set of values that can be turned into a
        Response object using `make_response`
        <https://flask.palletsprojects.com/en/1.1.x/api/#flask.make_response>.
    """
    request_data = request.get_data(as_text=True)
    name = request_data
    name = name[0:len(name)-1]
    client = bigquery.Client()
    Username = name
    HighScore = 1
    BanStatus = False
    query = "SELECT * FROM thermal-origin-372310.Player_Dataset.Players WHERE Username = '{}';".format(name)
    query_job = client.query(query)
    for row in query_job.result():
        Username = row.Username
        HighScore = row.HighScore
        BanStatus = row.BanStatus
    return '{} {} {}'.format(Username,HighScore,BanStatus)
