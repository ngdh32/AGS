import NavigationBar from './NavigationBar.js'
import { AGSContext } from '../AGSContext.js'

export function Master({children, agsContext}){
    return (
        <div>
            <AGSContext.Provider value={agsContext} >
                <header>
                    <NavigationBar/>
                </header>
                <main>
                    {children}
                </main>
            </AGSContext.Provider>
        </div>
    )
}