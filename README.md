This adds a simple GUI based search to your Tools dropdown menu that can be very useful for searching for prefabs. It returns clickable paths to prefabs as search results. Below is a description of all search options.

**Base Prefab:** This will search for any prefab variants that directly reference this prefab as their base prefab.

**Search Recursively:** This will modify the base prefab search to also include all child variants as well.

**Component Name:** This will search for all prefabs that use a specific component. While Unity's built in search can this, it is flawed in that it does not detect components on nested/child objects within the prefab. This search will also look at all child objects in every prefab.

**Filter By Layer:** Filters search result by defined physics layer.

**Layer:** A drop down to define the layer to filter by.

**Tag:** Filters by defined tag.

There's certainly a lot more functionality that could be added, such as negative searches, etc. But I don't currently have a use for them, so maybe later.
