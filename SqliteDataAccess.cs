using Dapper;
using ePollApi.Models;
using System.Data;
using System.Data.SQLite;

namespace ePollApi
{
    public class SqliteDataAccess
    {
        public static List<Poll> LoadPolls()
        {
            using (SQLiteConnection cnn = new SQLiteConnection("Data Source=./pollDb.db"))
            {
                var output = cnn.Query<Poll>("SELECT * FROM POLL", new DynamicParameters());
                return output.ToList();
            }
        }

        public static PollWithOptions LoadOne(int id)
        {
            using (SQLiteConnection cnn = new SQLiteConnection("Data Source=./pollDb.db"))
            {
                var outputPoll = cnn.Query<Poll>("SELECT * FROM POLL WHERE POLL.ID = @id", new { id });
                var outputOptions = cnn.Query<Option>("SELECT * FROM OPTION WHERE OPTION.POLLID = @id", new { id });

                var pwo = new PollWithOptions { Id = id, Title = outputPoll.First().Title, Options = outputOptions.ToList() };
                return pwo;
            }
        }

        public static PollWithOptions SavePoll(PollPost poll)
        {
            using (SQLiteConnection cnn = new SQLiteConnection("Data Source=./pollDb.db"))
            {
                cnn.Open();
                cnn.Execute("INSERT INTO POLL (TITLE) VALUES (@TITLE)", poll);
                long newRowId = cnn.LastInsertRowId;
                int newId = ((int)newRowId);

                foreach (var title in poll.Options)
                {
                    cnn.Execute("INSERT INTO OPTION (TITLE,VOTES,POLLID) VALUES (@title,@votes,@pollid)", new { title, votes=0, pollid=newId });
                }

                var outputPoll = cnn.Query<Poll>("SELECT * FROM POLL WHERE POLL.ID = @pollId", new { pollId=newId });
                var outputOptions = cnn.Query<Option>("SELECT * FROM OPTION WHERE OPTION.POLLID = @pollId", new { pollId=newId });
                var pwo = new PollWithOptions { Id = newId, Title = outputPoll.First().Title, Options = outputOptions.ToList() };
                cnn.Close();
                return pwo;
            }
        }

        public static PollWithOptions VotePoll(int pollId, int optionId)
        {
            using (SQLiteConnection cnn = new SQLiteConnection("Data Source=./pollDb.db"))
            {
                cnn.Open();
                cnn.Execute("UPDATE OPTION SET VOTES = VOTES + 1 WHERE ID = @optionId AND POLLID = @pollId", new { pollId, optionId });
                var outputPoll = cnn.Query<Poll>("SELECT * FROM POLL WHERE POLL.ID = @pollId", new { pollId });
                var outputOptions = cnn.Query<Option>("SELECT * FROM OPTION WHERE OPTION.POLLID = @pollId", new { pollId });
                cnn.Close();

                var pwo = new PollWithOptions { Id = pollId, Title = outputPoll.First().Title, Options = outputOptions.ToList() };
                return pwo;
            }
        }
    }
}
