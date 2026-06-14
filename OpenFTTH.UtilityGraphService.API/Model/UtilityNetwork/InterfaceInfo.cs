using System;

namespace OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork
{
    public record InterfaceInfo
    {
        public string InterfaceType { get; set; }
        public int SlotNumber { get; set; }
        public int SubSlotNumber { get; set; }
        public int PortNumber { get; set; }
        public string CircuitName { get; set; }

        public InterfaceInfo(string interfaceType, int slotNumber, int subSlotNumber, int portNumber, string circuitName)
        {
            InterfaceType = interfaceType;
            SlotNumber = slotNumber;
            SubSlotNumber = subSlotNumber;
            PortNumber = portNumber;
            CircuitName = circuitName;
        }

        public bool EqualTo(InterfaceInfo? otherInterfaceInfo)
        {
            if (otherInterfaceInfo is null)
                return false;

            if (this.InterfaceType == otherInterfaceInfo.InterfaceType &&
                this.SlotNumber == otherInterfaceInfo.SlotNumber &&
                this.SubSlotNumber == otherInterfaceInfo.SubSlotNumber &&
                this.PortNumber == otherInterfaceInfo.PortNumber &&
                this.CircuitName == otherInterfaceInfo.CircuitName
                )
                return true;

            return false;
        }

        public string GetName(bool withCircuitName = true)
        {
            string interfaceName = InterfaceType + "-" + SlotNumber;

            interfaceName += ("/" + SubSlotNumber);

            interfaceName += ("/" + PortNumber);

            if (CircuitName != null && withCircuitName)
                interfaceName += (" (" + CircuitName + ")");

            return interfaceName;
        }
    }
}
