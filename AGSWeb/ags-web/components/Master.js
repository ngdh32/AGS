import NavigationBar from './navigationBar.js'
import { AGSContext } from '../helpers/common/agsContext.js'

export function Master({children, agsContext}){
    return (
        <div>
            <AGSContext.Provider value={agsContext} >
                <header>
                    <NavigationBar/>
                </header>
                <main class="container-fluid">
                    {children}
                </main>
            </AGSContext.Provider>
        </div>
    )
}