import Head from 'next/head'
import styles from '../styles/Home.module.css'

export default function Home() {
  return (
    <div>
      <h1>Home page</h1>
      <h1>Home page 2</h1>
    </div>
  )
}

export async function getServerSideProps(context){
  console.log("index.js called");

  return {
    props: {
      message: "Home page"
    }
  }
}