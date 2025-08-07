using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System.Threading.Tasks;

namespace WhereAmIAgain;

public sealed class WhoAmIAgainPlugin : IDalamudPlugin {
    private readonly IClientState clientState;
    private readonly IDtrBar dtrBar;
    private readonly IFramework framework;
    
    private IDtrBarEntry? dtrBarEntry;
    
    public WhoAmIAgainPlugin(IClientState clientStateService, IFramework framework, IDtrBar dtrService) {
        this.framework = framework;
        clientState = clientStateService;
        dtrBar = dtrService;
        
        if (clientState.IsLoggedIn) framework.RunOnFrameworkThread(OnLogin);
        
        clientState.Login += OnLogin;
        clientState.Logout += OnLogout;
    }

    private void OnLogin() {
        if (dtrBar.Get("Who Am I Again?") is not { } entry) return;

        dtrBarEntry = entry;
        dtrBarEntry.Text = clientState.LocalPlayer?.Name;
        dtrBarEntry.Tooltip = new SeStringBuilder().AddText($"You are: {clientState.LocalPlayer?.Name ?? "Unknown"}").Build();
    }

    private void OnLogout(int type, int code) {
        dtrBarEntry?.Remove();
        dtrBarEntry = null;
    }

    public void Dispose() {
        clientState.Login -= OnLogin;
        clientState.Logout -= OnLogout;
    }
}
