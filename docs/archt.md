Architecture

```plantuml
interface HubApi
interface WebApi

component Hub
component Client

database Production

component DSVAdapter
component PostNordAdapter
component DHLAdapter
component SomethingElseAdapter


HubApi -right- Hub
WebApi -left- Hub

Client --> WebApi

DSVAdapter --> HubApi
PostNordAdapter --> HubApi
DHLAdapter --> HubApi
SomethingElseAdapter --> HubApi

Hub -- Production
```

Domain:

```plantuml

class Hub {}

class WarehouseSection {}

class Leverance {}

class LeveranceVogn {}



```
