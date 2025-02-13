﻿namespace OpenFTTH.UtilityGraphService.API.Commands
{
    public enum TerminalEquipmentErrorCodes
    {
        INVALID_TERMINAL_EQUIPMENT_ID_CANNOT_BE_EMPTY,
        INVALID_TERMINAL_EQUIPMENT_ID_ALREAD_EXISTS,
        INVALID_TERMINAL_EQUIPMENT_SPECIFICATION_ID_NOT_FOUND,
        INVALID_NODE_CONTAINER_ID_CANNOT_BE_EMPTY,
        NODE_CONTAINER_NOT_FOUND,
        INVALID_NUMBER_OF_EQUIPMENTS_VALUE,
        RACK_NOT_FOUND,
        INVALID_TERMINAL_EQUIPMENT_EXPECTED_RACK_EQUIPMENT,
        INVALID_TERMINAL_EQUIPMENT_EXPECTED_NON_RACK_EQUIPMENT,
        INVALID_RACK_UNIT_START_POSITION,
        TERMINAL_EQUIPMENT_NOT_FOUND_IN_ANY_RACK,
        INVALID_TERMINAL_STRUCTURE_SPECIFICATION_ID_NOT_FOUND,
        TERMINAL_EQUIPMENT_NOT_FOUND,
        TERMINAL_STRUCTURE_NOT_FOUND,
        CANNOT_REMOVE_TERMINAL_STRUCTURE_WITH_CONNECTED_TERMINALS,
        POSITION_ALREADY_OCCUPIED_BY_TERMINAL_STRUCTURE,
        TERMINAL_EQUIPMENT_DOES_NOT_FIT_IN_RACK,
        TERMINAL_EQUIPMENT_CANNOT_BE_MOVED_DOWN_DUE_TO_LACK_OF_FREE_SPACE,
    }
}
