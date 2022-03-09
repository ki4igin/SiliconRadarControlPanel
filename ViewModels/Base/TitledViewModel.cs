namespace SiliconRadarControlPanel.ViewModels.Base;

public abstract class TitledViewModel : ViewModelBase
{
    #region NotifyProperty <string> Title
    private string _title = string.Empty;
    public string Title { get => _title; set => Set(ref _title, value); }
    #endregion
}
