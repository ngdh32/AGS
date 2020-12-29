export default function callback (){
    return (
        <h1>Sign out successfully</h1>
    ) 
}


export async function getServerSideProps(context){
    // redirect back to the home page
    return {
        redirect: {
          permanent: false,
          destination: '/'
        }
    }
}