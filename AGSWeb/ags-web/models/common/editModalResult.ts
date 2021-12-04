export default class EditModalResult
{
    isSuccessful: boolean = false
    errorMessage?: string

    public static SetUnsuccessfulResponseWithErrorMessage(errorMessage: string) : EditModalResult
    {
        const result = new EditModalResult();
        result.isSuccessful = false;
        result.errorMessage = errorMessage;
        return result;
    }

    public static SetSuccessfulResponse() : EditModalResult
    {
        const result = new EditModalResult();
        result.isSuccessful = true;
        result.errorMessage = null;
        return result;
    }
}