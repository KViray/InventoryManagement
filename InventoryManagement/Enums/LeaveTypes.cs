using System.ComponentModel;

namespace InventoryManagement.Enums
{
    public enum LeaveTypes
    {
        CancelRequest,
        Emergency,
        DeathinImmediateFamily,
        DiscretionaryLeave,
        MaternityLeave,
        PaternityLeave,
        SickLeave,
        SoloParentLeave,
        VacationLeave,
        EOTMIncentiveLeave,
        VLBanding,
        [Description("LWOP-VL")]
        LWOPVL,
        [Description("LWOP-SL")]
        LWOPSL
    }
}
