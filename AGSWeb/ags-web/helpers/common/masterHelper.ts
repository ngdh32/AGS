import { GetUserInfo } from '../auth/authHelper'
import { GetLocaleCookieInServer } from './localizationHelper'
import { AGSContextModel} from './agsContext'
import { GetRedirectToErrorPageObject } from './utilityHelper'
import { IncomingMessage, ServerResponse } from 'http'
import { MasterPageDataType } from '../../models/pages/masterPageDataType'
import { PageDataType } from '../../models/pages/pageDataType'
import { GetServerSidePropsResult } from 'next'

// always call InitializePageWithMaster on each page to check
// 1. if the user has logged in
// 2. if the user has permission to view the page
// 3. construct correct page properties to the page js
// TODO: should move this part to new middleware
export async function InitializePageWithMaster(req: IncomingMessage, res: ServerResponse, callback: () =>  Promise<PageDataType>) : Promise<GetServerSidePropsResult<MasterPageDataType>>{
    // get claims from access token
    const userInfoClaims = await GetUserInfo(req, res);
    if (userInfoClaims == null){
        return {
            redirect: {
                permanent: false,
                destination: '/auth/login'
            }
        }
    }

    // get locale from cookie
    const locale = GetLocaleCookieInServer(req, res);
    let functionClaims: string[] = [];
    console.log({userInfoClaims})
    if (typeof userInfoClaims.FunctionClaim == "string")
    {
        functionClaims.push(userInfoClaims.FunctionClaim);
    } 

    if (Array.isArray(userInfoClaims.FunctionClaim))
    {
        functionClaims = functionClaims.concat(userInfoClaims.FunctionClaim);
    }

    // construct common context
    const agsContext = new AGSContextModel(
        userInfoClaims.sub
        , userInfoClaims.name
        , functionClaims
    , locale);

    console.log({agsContext})

    const pageDataResult = await callback();
    
    // add additional error code handling
    // if 403, no permission and send redirect object back to the page js
    if (pageDataResult.errorCode === 403){
        return GetRedirectToErrorPageObject();
    }

    return {
        props: {
            errorCode: pageDataResult.errorCode,
            agsContext: JSON.parse(JSON.stringify(agsContext)), 
            pageData: pageDataResult.pageData
        }
    }
}