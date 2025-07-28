namespace UserInterface
{
    public abstract class Presenter<T> where T : View
    {
        protected T View { get; }

        protected Presenter(T view)
        {
            View = view;
        }

        public void Show() => View.Show();

        public virtual void Hide() => View.Hide();
    }
}