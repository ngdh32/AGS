import Head from 'next/head'
import styles from '../styles/Home.module.css'
import { Master } from '../components/master.js'
import { AGSContext } from '../helpers/common/agsContext.js'
import { InitializePageWithMaster } from '../helpers/common/masterHelper.js'
import { useContext } from 'react'

export default function IndexUIWithMaster({agsContext, pageProps}){
  return (
    <Master agsContext={agsContext}>
      <IndexUI {...pageProps}/>
    </Master>
  )
}

function IndexUI (){
  const agsContext = useContext(AGSContext);
  const functionClaims = agsContext.functionClaims.map(x => {
    return (
      <li class="list-group-item">
        {x}
      </li>
    )
  })
  
  return (
    <div>
      <h1>Username: {agsContext.username}</h1>
      <h1>Locale: {agsContext.locale}</h1>
      <h1>Function Claims:</h1>
      <ul class="list-group">
        {functionClaims}
      </ul>
    </div>
  )
}

export async function getServerSideProps(context){
  const result = await InitializePageWithMaster(context.req, context.res, async () => {
    return {
      
    }
  })

  console.log({result})
  return result;
}