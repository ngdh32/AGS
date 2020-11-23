export default function AboutMe(){
    console.log("Aboue me")

    return (
        <h1>
            About Me!
        </h1>
    )
}

export async function getServerSideProps(context){
    console.log("Aboue mE")

    return {
        props: {}
    }
}