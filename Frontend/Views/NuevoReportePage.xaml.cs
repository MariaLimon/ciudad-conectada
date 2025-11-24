using Frontend.ViewModels;

namespace Frontend.Views
{
    public partial class NuevoReportePage : ContentPage
    {
        public NuevoReportePage(NuevoReporteViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}