using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server;

namespace dotnetfeatures
{
    /// <summary>
    /// Demonstrates circuit/service state persistence. The service stores a user name and a hit count
    /// and persists them via <see cref="PersistentComponentState"/> so that the values survive
    /// the transition from prerendered to interactive and circuit reconnects.
    /// </summary>
    public class CustomUserService
    {
        private readonly PersistentComponentState _persistentState;
        private bool _isRestoreAttempted;

        private const string UserNameKey = "CustomUserService.UserName";
        private const string HitCountKey = "CustomUserService.HitCount";

        public string? UserName { get; private set; }
        [PersistentState]
        public int HitCount { get; set; }
    }
}
