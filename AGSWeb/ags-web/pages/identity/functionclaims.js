import { Master } from '../../components/Master.js'
import { AGSContext } from '../../AGSContext.js'
import { InitializePageWithMaster } from '../../services/Helpers/Master.js'
import { useContext } from 'react'
import { GetFunctionClaims } from '../../services/Helpers/Identity/functionClaims.js'

export default function FunctionClaimUIWithMaster({agsContext, pageProps}){
    return (
      <Master agsContext={agsContext}>
        <FunctionClaimUI {...pageProps}/>
      </Master>
    )
  }
  
  function FunctionClaimUI ({functionClaims}){
    const agsContext = useContext(AGSContext);
    
    return (
      <div>
          <h1>{JSON.stringify(functionClaims)}</h1>
      </div>
    )
  }
  
  export async function getServerSideProps(context){
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const functionClaims = await GetFunctionClaims(context.req, context.res);
        console.log(functionClaims)
        return {
            functionClaims
        }
    })
  
    return result;
  }