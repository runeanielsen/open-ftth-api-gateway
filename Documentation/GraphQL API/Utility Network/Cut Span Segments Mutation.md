## Example Mutation

```graphql
mutation {
  spanEquipment {
    cutSpanSegments(
      routeNodeId: "020c3eb8-13cc-4c0e-a331-c7610f996a52"
      spanSegmentstoCut: [
        "59bfec79-f22b-4e0a-9818-d0ef6b7961e5"
        "ebe26aff-f135-4eed-9a7a-1ef397f4ee9f"
      ]
    ) {
      isSuccess
      errorCode
    }
  }
}
```

## Params
All are mandatory.


## Error Codes
[Error Codes](https://github.com/DAXGRID/open-ftth-utility-graph-service/blob/master/OpenFTTH.UtilityGraphService.API/Commands/CutSpanSegmentsAtRouteNodeErrorCodes.cs)





