/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
/* eslint-disable @typescript-eslint/no-explicit-any */

import 'reflect-metadata';

import { injectable } from 'inversify';

export enum ContentType {
  FormData,
  Json,
}

export enum MethodType {
  DELETE,
  GET,
  PATCH,
  POST,
  PUT,
}

export interface ApiHeader {
  contentType?: ContentType;
  authorization?: string | null;
  accessControlAllowOrigin?: string | null;
}

export interface ApiResponse<T> {
  status: number;
  statusText: string;
  headers: Headers;
  data?: T;
}

export interface HttpService {
  sendAsync<T>(route: string, methodType: MethodType, headers?: ApiHeader, data?: any): Promise<ApiResponse<T>>;
}

@injectable()
export default class DefaultHttpService implements HttpService {
  private readonly headerKeyContentType = 'Content-Type';
  private readonly headerValueContentTypeJson = 'application/json';
  private readonly headerValueContentTypeFormData = 'application/x-www-form-urlencoded';

  private readonly headerValueMethodTypeDelete = 'DELETE';
  private readonly headerValueMethodTypeGet = 'GET';
  private readonly headerValueMethodTypePatch = 'PATCH';
  private readonly headerValueMethodTypePost = 'POST';
  private readonly headerValueMethodTypePut = 'PUT';

  private readonly headerValueCredentialsTypeInclude = 'include';
  private readonly headerValueCredentialsTypeOmit = 'omit';

  public async sendAsync<T>(
    route: string,
    methodType: MethodType,
    headers?: ApiHeader,
    data?: any
  ): Promise<ApiResponse<T>> {
    const headersRequest = this.getHeaders(headers);
    const bodyRequest = this.getBody(data, headers);
    const method = this.getMethod(methodType);
    const credentials = this.getCredentials(headers);
    const requestOptions: RequestInit = {
      method,
      credentials,
      headers: headersRequest,
      body: bodyRequest,
    };
    const response = await fetch(`${route}`, requestOptions);

    return this.handleResponse(response);
  }

  private async handleResponse<T>(response: Response): Promise<ApiResponse<T>> {
    const result: ApiResponse<T> = {
      ...response,
      status: response.status,
      statusText: response.statusText,
      headers: response.headers,
    };

    const contentType = response.headers.get('content-type');

    if (contentType && contentType.includes('application/json')) {
      const responseText = await response.text();

      try {
        const responseData = JSON.parse(responseText);
        result.data = responseData;
      } catch (error: unknown) {
        console.log(`Did not receive JSON, instead received:\n"${responseText}"`);

        if (error instanceof Error) {
          console.error(error.message);
        } else {
          console.log(error);
        }
      }
    }

    return result;
  }

  private getCredentials = (headers?: ApiHeader): RequestCredentials => {
    return headers && !!headers.authorization
      ? this.headerValueCredentialsTypeInclude
      : this.headerValueCredentialsTypeOmit;
  };

  private getMethod = (method: MethodType): string => {
    switch (method) {
      case MethodType.DELETE: {
        return this.headerValueMethodTypeDelete;
      }

      case MethodType.PATCH: {
        return this.headerValueMethodTypePatch;
      }

      case MethodType.POST: {
        return this.headerValueMethodTypePost;
      }

      case MethodType.PUT: {
        return this.headerValueMethodTypePut;
      }

      default: {
        return this.headerValueMethodTypeGet;
      }
    }
  };

  private getBody = (data?: any, headers?: ApiHeader): string | URLSearchParams | undefined => {
    if (data) {
      if (headers && headers.contentType === ContentType.Json) {
        return JSON.stringify(data);
      } else {
        const parameters = new URLSearchParams();

        for (const key of Object.keys(data)) {
          parameters.append(key, data[key]);
        }

        return parameters;
      }
    }

    return undefined;
  };

  private getHeaders = (headers?: ApiHeader): Record<string, string> => {
    let headersRequest = {};

    if (headers) {
      if (headers.contentType !== undefined) {
        headersRequest = {
          ...headersRequest,
          [this.headerKeyContentType]:
            headers.contentType === ContentType.Json
              ? this.headerValueContentTypeJson
              : this.headerValueContentTypeFormData,
        };
      }

      if (headers.accessControlAllowOrigin) {
        headersRequest = {
          ...headersRequest,
          'Access-Control-Allow-Origin': headers.accessControlAllowOrigin,
        };
      }

      if (headers.authorization) {
        headersRequest = {
          ...headersRequest,
          Authorization: `Bearer ${headers.authorization}`,
        };
      }
    }

    return headersRequest;
  };
}
