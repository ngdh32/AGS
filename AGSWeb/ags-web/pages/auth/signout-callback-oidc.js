import { useEffect } from "react"

export default function callback (){
    useEffect(() => {
        setTimeout(() => { window.location = "/" }, 1000);
    }, [])

    return (
        <h1>Sign out successfully</h1>
    ) 
}