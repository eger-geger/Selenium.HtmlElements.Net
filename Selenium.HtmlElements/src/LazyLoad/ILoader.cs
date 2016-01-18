namespace HtmlElements.LazyLoad
{
    internal interface ILoader<TObject>
    {
        TObject Load();

        void Reset();

        TObject ResetAndLoad();
    }
}