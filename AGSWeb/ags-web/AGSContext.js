import { createContext } from "react";
import { default_locale } from './config/Localization.js'

export class AGSContextModel{
    constructor(userId, username, functionClaims, locale){
        this.userId = userId;
        this.username = username;
        this.functionClaims = functionClaims;
        this.locale = locale;
    }
}

const defaultAGSContext = new AGSContextModel("", "", [], default_locale.value);

export const AGSContext = createContext(defaultAGSContext)