using System.Collections.Generic;

namespace ArmsModels.Shared
{
    public class RoleClaimsList
    {
        public string DocType { get; set; }
        public string ClaimType { get; set; }
        public List<string> ClaimValues { get; set; } = new();
    }
}
