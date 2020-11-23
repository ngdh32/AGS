import App from 'next/app'
import menuOptions from '../menu'
import 'bootstrap/dist/css/bootstrap.min.css'
import NavigationBar from '../components/NavigationBar'

function MyApp({ Component, pageProps, menuOptions }) {
  if (menuOptions == undefined || menuOptions.length == 0){
    return (
      <Component {...pageProps} />
    )
  }else {
    return (
      <div>
        <header>
          <NavigationBar menuOptions={menuOptions} />
        </header>
        <main>
          <Component {...pageProps} />
        </main>
      </div>
      
    )
  }
}

MyApp.getInitialProps = async (appContext) => {
  console.log("_app.js called")
  // calls page's `getInitialProps` and fills `appProps.pageProps`
  const appProps = await App.getInitialProps(appContext);
  appProps.menuOptions = menuOptions;
  
  return { ...appProps }
}

async function GetMenuOptions(){

}

export default MyApp
