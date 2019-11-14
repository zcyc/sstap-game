namespace SSTap.Model
{
    internal class Notice
    {
        public string title;
        public string content;
        public long time;

        public Notice()
        {
            this.time = 0L;
            this.content = "";
            this.title = "";
        }
    }
}
