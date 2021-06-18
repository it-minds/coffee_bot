/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.9.4.0 (NJsonSchema v10.3.1.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

export class AuthBase {
  private accessToken: string;
  constructor(accessToken: string) {
    this.accessToken = accessToken;
  }

  transformHttpRequestOptions(options: RequestInit): Promise<RequestInit> {
    if (options.headers && this.accessToken) {
      (<Record<string, string>>options.headers).Authorization =
        "Bearer " + this.accessToken;
      return Promise.resolve(options);
    }
    return Promise.resolve(options);
  }
}

export class ClientBase {
  constructor(private AuthBase: AuthBase) {}

  private cacheableResponse = false;
  private cacheStrategy: "CacheFirst" | "NetworkFirst" = "NetworkFirst";
  private cacheAllowStatuses: number[] = [200];
  private cacheableOptions: RequestInit = null;

  setCacheableResponse(
    cacheStrategy: ClientBase["cacheStrategy"] = "NetworkFirst",
    cacheAllowStatuses: ClientBase["cacheAllowStatuses"] = [200]
  ) {
    this.cacheableResponse = true;
    this.cacheStrategy = cacheStrategy;
    this.cacheAllowStatuses = cacheAllowStatuses;
  }

  async transformOptions(options: RequestInit): Promise<RequestInit> {
    const result = await (this.AuthBase
      ? this.AuthBase.transformHttpRequestOptions(options)
      : Promise.resolve(options));

    if (this.cacheableResponse) {
      this.cacheableOptions = result;
    }

    return result;
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

  async transformResult(
    url: string,
    networkResponse: Response,
    cb: (response: Response) => any
  ) {
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
    return cb(response);
  }
}

export interface IAuthClient {
    checkAuth(): Promise<AuthUser>;
    login(): Promise<boolean>;
    loginCallback(code?: string | null | undefined, state?: string | null | undefined): Promise<boolean>;
}

export class AuthClient extends ClientBase implements IAuthClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(configuration: AuthBase, baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(configuration);
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    checkAuth(): Promise<AuthUser> {
        let url_ = this.baseUrl + "/api/Auth";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "PUT",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processCheckAuth(_response));
        });
    }

    protected processCheckAuth(response: Response): Promise<AuthUser> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = AuthUser.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<AuthUser>(<any>null);
    }

    login(): Promise<boolean> {
        let url_ = this.baseUrl + "/api/Auth/login";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processLogin(_response));
        });
    }

    protected processLogin(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(<any>null);
    }

    loginCallback(code?: string | null | undefined, state?: string | null | undefined): Promise<boolean> {
        let url_ = this.baseUrl + "/api/Auth/login-callback?";
        if (code !== undefined && code !== null)
            url_ += "code=" + encodeURIComponent("" + code) + "&";
        if (state !== undefined && state !== null)
            url_ += "state=" + encodeURIComponent("" + state) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processLoginCallback(_response));
        });
    }

    protected processLoginCallback(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(<any>null);
    }
}

export interface IChannelClient {
    getMyChannels(): Promise<ChannelSettingsIdDto[]>;
    updateChannelState(command: UpdateChannelPauseCommand): Promise<FileResponse>;
}

export class ChannelClient extends ClientBase implements IChannelClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(configuration: AuthBase, baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(configuration);
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    getMyChannels(): Promise<ChannelSettingsIdDto[]> {
        let url_ = this.baseUrl + "/api/Channel/MyChannels";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processGetMyChannels(_response));
        });
    }

    protected processGetMyChannels(response: Response): Promise<ChannelSettingsIdDto[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(ChannelSettingsIdDto.fromJS(item));
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ChannelSettingsIdDto[]>(<any>null);
    }

    updateChannelState(command: UpdateChannelPauseCommand): Promise<FileResponse> {
        let url_ = this.baseUrl + "/api/Channel/UpdateChannelState";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(command);

        let options_ = <RequestInit>{
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/octet-stream"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processUpdateChannelState(_response));
        });
    }

    protected processUpdateChannelState(response: Response): Promise<FileResponse> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200 || status === 206) {
            const contentDisposition = response.headers ? response.headers.get("content-disposition") : undefined;
            const fileNameMatch = contentDisposition ? /filename="?([^"]*?)"?(;|$)/g.exec(contentDisposition) : undefined;
            const fileName = fileNameMatch && fileNameMatch.length > 1 ? fileNameMatch[1] : undefined;
            return response.blob().then(blob => { return { fileName: fileName, data: blob, status: status, headers: _headers }; });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<FileResponse>(<any>null);
    }
}

export interface IHealthClient {
    getBackendHealth(): Promise<boolean>;
}

export class HealthClient extends ClientBase implements IHealthClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(configuration: AuthBase, baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(configuration);
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    getBackendHealth(): Promise<boolean> {
        let url_ = this.baseUrl + "/api/Health";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processGetBackendHealth(_response));
        });
    }

    protected processGetBackendHealth(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(<any>null);
    }
}

export interface ITestClient {
    channelSync(command: SyncronizeChannelsCommand): Promise<boolean>;
    newChannelMessager(command: NewChannelMessagerCommand): Promise<boolean>;
    roundInitiator(command: RoundInitiatorCommand): Promise<boolean>;
}

export class TestClient extends ClientBase implements ITestClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(configuration: AuthBase, baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(configuration);
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    channelSync(command: SyncronizeChannelsCommand): Promise<boolean> {
        let url_ = this.baseUrl + "/api/Test/channel-sync";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(command);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processChannelSync(_response));
        });
    }

    protected processChannelSync(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(<any>null);
    }

    newChannelMessager(command: NewChannelMessagerCommand): Promise<boolean> {
        let url_ = this.baseUrl + "/api/Test/new-channel-msg";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(command);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processNewChannelMessager(_response));
        });
    }

    protected processNewChannelMessager(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(<any>null);
    }

    roundInitiator(command: RoundInitiatorCommand): Promise<boolean> {
        let url_ = this.baseUrl + "/api/Test/round-init";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(command);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processRoundInitiator(_response));
        });
    }

    protected processRoundInitiator(response: Response): Promise<boolean> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<boolean>(<any>null);
    }
}

export class AuthUser implements IAuthUser {
    slackUserId?: string | null;
    email?: string | null;

    constructor(data?: IAuthUser) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.slackUserId = _data["slackUserId"] !== undefined ? _data["slackUserId"] : <any>null;
            this.email = _data["email"] !== undefined ? _data["email"] : <any>null;
        }
    }

    static fromJS(data: any): AuthUser {
        data = typeof data === 'object' ? data : {};
        let result = new AuthUser();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["slackUserId"] = this.slackUserId !== undefined ? this.slackUserId : <any>null;
        data["email"] = this.email !== undefined ? this.email : <any>null;
        return data; 
    }
}

export interface IAuthUser {
    slackUserId?: string | null;
    email?: string | null;
}

export class ChannelSettingsDto implements IChannelSettingsDto {
    groupSize?: number;
    startsDay?: DayOfWeek;
    weekRepeat?: number;
    durationInDays?: number;
    individualMessage?: boolean;

    constructor(data?: IChannelSettingsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.groupSize = _data["groupSize"] !== undefined ? _data["groupSize"] : <any>null;
            this.startsDay = _data["startsDay"] !== undefined ? _data["startsDay"] : <any>null;
            this.weekRepeat = _data["weekRepeat"] !== undefined ? _data["weekRepeat"] : <any>null;
            this.durationInDays = _data["durationInDays"] !== undefined ? _data["durationInDays"] : <any>null;
            this.individualMessage = _data["individualMessage"] !== undefined ? _data["individualMessage"] : <any>null;
        }
    }

    static fromJS(data: any): ChannelSettingsDto {
        data = typeof data === 'object' ? data : {};
        let result = new ChannelSettingsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["groupSize"] = this.groupSize !== undefined ? this.groupSize : <any>null;
        data["startsDay"] = this.startsDay !== undefined ? this.startsDay : <any>null;
        data["weekRepeat"] = this.weekRepeat !== undefined ? this.weekRepeat : <any>null;
        data["durationInDays"] = this.durationInDays !== undefined ? this.durationInDays : <any>null;
        data["individualMessage"] = this.individualMessage !== undefined ? this.individualMessage : <any>null;
        return data; 
    }
}

export interface IChannelSettingsDto {
    groupSize?: number;
    startsDay?: DayOfWeek;
    weekRepeat?: number;
    durationInDays?: number;
    individualMessage?: boolean;
}

export class ChannelSettingsIdDto extends ChannelSettingsDto implements IChannelSettingsIdDto {
    id?: number;
    slackChannelId?: string | null;
    slackChannelName?: string | null;
    paused?: boolean;

    constructor(data?: IChannelSettingsIdDto) {
        super(data);
    }

    init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.slackChannelId = _data["slackChannelId"] !== undefined ? _data["slackChannelId"] : <any>null;
            this.slackChannelName = _data["slackChannelName"] !== undefined ? _data["slackChannelName"] : <any>null;
            this.paused = _data["paused"] !== undefined ? _data["paused"] : <any>null;
        }
    }

    static fromJS(data: any): ChannelSettingsIdDto {
        data = typeof data === 'object' ? data : {};
        let result = new ChannelSettingsIdDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["slackChannelId"] = this.slackChannelId !== undefined ? this.slackChannelId : <any>null;
        data["slackChannelName"] = this.slackChannelName !== undefined ? this.slackChannelName : <any>null;
        data["paused"] = this.paused !== undefined ? this.paused : <any>null;
        super.toJSON(data);
        return data; 
    }
}

export interface IChannelSettingsIdDto extends IChannelSettingsDto {
    id?: number;
    slackChannelId?: string | null;
    slackChannelName?: string | null;
    paused?: boolean;
}

export enum DayOfWeek {
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
}

export class UpdateChannelPauseCommand implements IUpdateChannelPauseCommand {
    input?: UpdateChannelPauseInput | null;

    constructor(data?: IUpdateChannelPauseCommand) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
            this.input = data.input && !(<any>data.input).toJSON ? new UpdateChannelPauseInput(data.input) : <UpdateChannelPauseInput>this.input; 
        }
    }

    init(_data?: any) {
        if (_data) {
            this.input = _data["input"] ? UpdateChannelPauseInput.fromJS(_data["input"]) : <any>null;
        }
    }

    static fromJS(data: any): UpdateChannelPauseCommand {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateChannelPauseCommand();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["input"] = this.input ? this.input.toJSON() : <any>null;
        return data; 
    }
}

export interface IUpdateChannelPauseCommand {
    input?: IUpdateChannelPauseInput | null;
}

export class UpdateChannelPauseInput implements IUpdateChannelPauseInput {
    slackUserId?: string | null;
    channelId?: number;
    paused?: boolean;

    constructor(data?: IUpdateChannelPauseInput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.slackUserId = _data["slackUserId"] !== undefined ? _data["slackUserId"] : <any>null;
            this.channelId = _data["channelId"] !== undefined ? _data["channelId"] : <any>null;
            this.paused = _data["paused"] !== undefined ? _data["paused"] : <any>null;
        }
    }

    static fromJS(data: any): UpdateChannelPauseInput {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateChannelPauseInput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["slackUserId"] = this.slackUserId !== undefined ? this.slackUserId : <any>null;
        data["channelId"] = this.channelId !== undefined ? this.channelId : <any>null;
        data["paused"] = this.paused !== undefined ? this.paused : <any>null;
        return data; 
    }
}

export interface IUpdateChannelPauseInput {
    slackUserId?: string | null;
    channelId?: number;
    paused?: boolean;
}

export class SyncronizeChannelsCommand implements ISyncronizeChannelsCommand {

    constructor(data?: ISyncronizeChannelsCommand) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
    }

    static fromJS(data: any): SyncronizeChannelsCommand {
        data = typeof data === 'object' ? data : {};
        let result = new SyncronizeChannelsCommand();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        return data; 
    }
}

export interface ISyncronizeChannelsCommand {
}

export class NewChannelMessagerCommand implements INewChannelMessagerCommand {
    slackChannelId?: string | null;

    constructor(data?: INewChannelMessagerCommand) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.slackChannelId = _data["slackChannelId"] !== undefined ? _data["slackChannelId"] : <any>null;
        }
    }

    static fromJS(data: any): NewChannelMessagerCommand {
        data = typeof data === 'object' ? data : {};
        let result = new NewChannelMessagerCommand();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["slackChannelId"] = this.slackChannelId !== undefined ? this.slackChannelId : <any>null;
        return data; 
    }
}

export interface INewChannelMessagerCommand {
    slackChannelId?: string | null;
}

export class RoundInitiatorCommand implements IRoundInitiatorCommand {

    constructor(data?: IRoundInitiatorCommand) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
    }

    static fromJS(data: any): RoundInitiatorCommand {
        data = typeof data === 'object' ? data : {};
        let result = new RoundInitiatorCommand();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        return data; 
    }
}

export interface IRoundInitiatorCommand {
}

export enum CommandErrorCode {
    AbstractComparisonValidator = 0,
    AsyncPredicateValidator = 1,
    AsyncValidatorBase = 2,
    ChildValidatorAdaptor = 3,
    CreditCardValidator = 4,
    CustomValidator = 5,
    EmailValidator = 6,
    EmptyValidator = 7,
    EnumValidator = 8,
    EqualValidator = 9,
    ExclusiveBetweenValidator = 10,
    GreaterThanOrEqualValidator = 11,
    GreaterThanValidator = 12,
    IPropertyValidator = 13,
    InclusiveBetweenValidator = 14,
    LengthValidator = 15,
    LessThanOrEqualValidator = 16,
    LessThanValidator = 17,
    NoopPropertyValidator = 18,
    NotEmptyValidator = 19,
    NotEqualValidator = 20,
    NotNullValidator = 21,
    NullValidator = 22,
    OnFailureValidator = 23,
    PolymorphicValidator = 24,
    PredicateValidator = 25,
    PropertyValidator = 26,
    PropertyValidatorContext = 27,
    RegularExpressionValidator = 28,
    ScalePrecisionValidator = 29,
    StringEnumValidator = 30,
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}

export class SwaggerException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new SwaggerException(message, status, response, headers, null);
}

/* istanbul ignore file */