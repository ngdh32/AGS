import Head from 'next/head'
import styles from '../styles/Home.module.css'
import { Master } from '../components/Master.js'
import { AGSContext } from '../AGSContext.js'
import { InitializePageWithMaster } from '../services/Helpers/Master.js'
import { useContext } from 'react'

export default function IndexUIWithMaster({agsContext, pageProps}){
  return (
    <Master agsContext={agsContext}>
      <IndexUI {...pageProps}/>
    </Master>
  )
}

function IndexUI ({message}){
  const agsContext = useContext(AGSContext);
  
  return (
    <div>
      <h1>Message: {message}</h1>
      <h1>Username: {agsContext.username}</h1>
      <h1>Locale: {agsContext.locale}</h1>
    </div>
  )
}

export async function getServerSideProps(context){
  const result = await InitializePageWithMaster(context.req, context.res, async () => {
    return {
      message: "Hello World"
    }
  })

  return result;
}