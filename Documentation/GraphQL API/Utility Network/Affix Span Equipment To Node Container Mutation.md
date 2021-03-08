## Example Mutation

```graphql
mutation
{
  spanEquipment
  {
    affixSpanEquipmentToNodeContainer(
      spanSegmentId: "15543190-752d-4651-9cf0-25ed9de038f0"
      nodeContainerId: "5bf6561c-ba6e-4eb9-b6fc-494c3fa90a64"
      nodeContainerSide: WEST
    )
    {
      isSuccess
      errorCode
    }
  }
}
```

## Params
All are mandatory.


## Error Codes
[Error Codes](https://github.com/DAXGRID/open-ftth-utility-graph-service/blob/master/OpenFTTH.UtilityGraphService.API/Commands/AffixSpanEquipmentToNodeContainerErrorCodes.cs)





