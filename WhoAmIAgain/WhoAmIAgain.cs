using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System;

namespace WhereAmIAgain;

public sealed class WhoAmIAgainPlugin : IDalamudPlugin {
    private readonly IClientState clientState;
    private readonly IDtrBar dtrBar;
    
    private IDtrBarEntry? dtrBarEntry;
    
    public WhoAmIAgainPlugin(IClientState clientStateService, IDtrBar dtrService) {
        clientState = clientStateService;
        dtrBar = dtrService;
        
        if (clientState.IsLoggedIn) OnLogin();
        
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
