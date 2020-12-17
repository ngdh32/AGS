import { GetUserInfo } from '../Auth/Client.js'
import { GetLocaleCookieInServer } from '../Common/Localization.js'
import { AGSContextModel} from '../../AGSContext.js'

export async function InitializePageWithMaster(req, res, callback){
    const userInfoClaims = await GetUserInfo(req, res);
    if (userInfoClaims == null){
        return {
            redirect: {
                permanent: false,
                destination: '/auth/login'
            }
        }
    }

    const locale = GetLocaleCookieInServer(req, res);
    const agsContext = new AGSContextModel(
        userInfoClaims.sub
        , userInfoClaims.name
        , userInfoClaims.FunctionClaim
    , locale);

    const pageProps = await callback();
    return {
        props: {
            agsContext: JSON.parse(JSON.stringify(agsContext)), 
            pageProps:JSON.parse(JSON.stringify(pageProps))
        }
    }
}