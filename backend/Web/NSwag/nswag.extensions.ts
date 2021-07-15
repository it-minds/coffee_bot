/* istanbul ignore file */

/**
 * Used during client configuration.
 */
export class ClientConfiguration {
  constructor(public accessToken: string) {}
}

/**
 * Any public method are meant to be used after the individual client has been initialized
 */
export class ClientBase {
  constructor(private clientConfiguration: ClientConfiguration) {}
  private cacheableResponse = false;
  private cacheStrategy: "CacheFirst" | "NetworkFirst" = "NetworkFirst";
  private cacheAllowStatuses: number[] = [200];
  private cacheableOptions: RequestInit = null;
  private responseCallbackMap: Record<number, () => void | Promise<void>> =
    null;

  public setCacheableResponse(
    cacheStrategy: ClientBase["cacheStrategy"] = "NetworkFirst",
    cacheAllowStatuses: ClientBase["cacheAllowStatuses"] = [200]
  ) {
    this.cacheableResponse = true;
    this.cacheStrategy = cacheStrategy;
    this.cacheAllowStatuses = cacheAllowStatuses;
  }

  public setStatusCallbackMap(
    responseCallbackMap: ClientBase["responseCallbackMap"]
  ) {
    this.responseCallbackMap = responseCallbackMap;
  }

  protected async transformOptions(options: RequestInit): Promise<RequestInit> {
    if (options.headers && this.clientConfiguration.accessToken) {
      (options.headers as Record<string, string>)["Authorization"] =
        "Bearer " + this.clientConfiguration.accessToken;
    }

    if (this.cacheableResponse) {
      this.cacheableOptions = options;
    }

    return options;
  }

  protected async transformResult(
    url: string,
    networkResponse: Response,
    cb: (response: Response) => Promise<any>
  ) {
    const response = await this.checkCache(url, networkResponse);
    const hasBeenHandled = await this.checkStatusCallback(response);

    if (hasBeenHandled) {
      return cb(response).catch((err) => {
        console.error("nswag status had been handled", response.status);
        console.error(err);
        return null;
      });
    }
    return cb(response);
  }

  private async cacheResponse(
    request: Request,
    response: Response
  ): Promise<Response> {
    const cache = await caches.open("nswagts.v1");
    const cloned = response.clone();
    await cache.put(request, response);

    return cloned;
  }

  private async checkCache(url: string, networkResponse: Response) {
    let response: Response = networkResponse;
    if (process.browser && this.cacheableResponse) {
      console.debug("NswagTs transformResult cacheableResponse executing...");
      const request = new Request(url, this.cacheableOptions);

      const cacheResponse = await caches.match(request);

      const networkOk = this.cacheAllowStatuses.includes(
        networkResponse?.status ?? 0
      );
      const cacheOk = this.cacheAllowStatuses.includes(
        cacheResponse?.status ?? 0
      );

      if (this.cacheStrategy === "CacheFirst") {
        if (cacheOk) {
          console.debug(
            "NswagTs transformResult cacheableResponse cache first using cache",
            cacheResponse
          );
          response = cacheResponse;
        } else {
          console.debug(
            "NswagTs transformResult cacheableResponse cache first using network",
            networkResponse
          );
          response = networkOk
            ? await this.cacheResponse(request, networkResponse)
            : networkResponse;
        }
      } else if (this.cacheStrategy === "NetworkFirst") {
        if (networkOk) {
          console.debug(
            "NswagTs transformResult cacheableResponse network first using network ok",
            networkResponse
          );
          response = await this.cacheResponse(request, networkResponse);
        } else if (cacheOk) {
          console.debug(
            "NswagTs transformResult cacheableResponse network first using cache",
            cacheResponse
          );
          response = cacheResponse;
        } else {
          console.debug(
            "NswagTs transformResult cacheableResponse network first using network failure",
            networkResponse
          );
          response = networkResponse;
        }
      }
    }
    this.cacheableResponse = false;
    return response;
  }

  private async checkStatusCallback(response: Response): Promise<boolean> {
    if (this.responseCallbackMap == null) return false;

    if (
      Object.keys(this.responseCallbackMap).includes(response.status.toString())
    ) {
      const db = this.responseCallbackMap[response.status];

      await db();

      return true;
    }

    return false;
  }
}
