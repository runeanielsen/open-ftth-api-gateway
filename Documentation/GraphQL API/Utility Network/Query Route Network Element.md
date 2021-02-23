## Example Query

```graphql
query {
  routeNetwork {
    routeElement(id: "53fd280f-f608-4f67-8939-91be2756cb78") {
      kind
      routeNodeInfo {
        function
        kind
      }
      routeSegmentInfo {
        kind
        width
        height
      }
      namingInfo {
        name
        description
      }
      lifecycleInfo {
        deploymentState
        installationDate
        removalDate
      }
      mappingInfo {
        method
        horizontalAccuracy
        verticalAccuracy
        sourceInfo
        surveyDate
      }
      safetyInfo {
        classification
        remark
      }
    }
  }
}
```