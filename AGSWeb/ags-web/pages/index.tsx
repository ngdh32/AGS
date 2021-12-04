import Head from 'next/head'
import styles from '../styles/Home.module.css'
import { Master } from '../components/master'
import { AGSContext } from '../helpers/common/agsContext'
import { InitializePageWithMaster } from '../helpers/common/masterHelper'
import { useContext } from 'react'
import { GetServerSidePropsResult, NextApiRequest, NextApiResponse } from 'next'
import { MasterPageDataType } from '../models/pages/masterPageDataType'
import { errorCodeSuccess } from '../config/common'

export default function IndexUIWithMaster({agsContext, pageData}: MasterPageDataType){
  return (
    <Master agsContext={agsContext}>
      <IndexUI {...pageData}/>
    </Master>
  )
}

function IndexUI (){
  const agsContext = useContext(AGSContext);
  console.log({agsContext})
  const functionClaims = agsContext.functionClaims.map(x => {
    return (
      <li className="list-group-item">
        {x}
      </li>
    )
  })
  
  return (
    <div>
      <h1>Username: {agsContext.username}</h1>
      <h1>Locale: {agsContext.locale}</h1>
      <h1>Function Claims:</h1>
      <ul className="list-group">
        {functionClaims}
      </ul>
    </div>
  )
}

export async function getServerSideProps(context: { req: NextApiRequest; res: NextApiResponse; }) : Promise<GetServerSidePropsResult<MasterPageDataType>> {
  const result = await InitializePageWithMaster(context.req, context.res, async () => {
    return {
        errorCode: errorCodeSuccess,
        pageData: null
    }
  })

  return result;
}