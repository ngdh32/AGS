import { createContext } from "react";
import { default_locale } from '../../config/localization'

export class AGSContextModel{
    userId: string
    username: string
    functionClaims: string[]
    locale: string
    

    constructor(userId: string, username: string, functionClaims: string[], locale: string){
        this.userId = userId;
        this.username = username;
        this.functionClaims = functionClaims;
        this.locale = locale;
    }
}

const defaultAGSContext = new AGSContextModel("", "", [], default_locale.value);

export const AGSContext = createContext(defaultAGSContext)