namespace HtmlElements.LazyLoad
{
    internal abstract class TransparentLoader<TObject> : ILoader<TObject> where TObject : class
    {
        public abstract TObject Load();

        public void Reset()
        {
        }

        public TObject ResetAndLoad()
        {
            return Load();
        }
    }
}