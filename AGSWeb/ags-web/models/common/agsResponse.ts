import { AxiosResponse } from "axios"

export default class AGSResponse
{
    isSuccessful: boolean = false
    responseCode?: number
    data?: any

    SetSuccessfulResponse(response: AxiosResponse) : void
    {
        this.isSuccessful = true;
        this.data = response.data.data;
        this.responseCode = response.data.code;
    }

    SetUnsuccessfulResponseWithError(err: any) : void
    {
        this.isSuccessful = false;
        if (err.response.data != null)
        {
            this.data = err.response.data.data;
            this.responseCode = err.response.data.code;
        }else {
            this.responseCode = err.response.status;
        }
    }
}