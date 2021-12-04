import NavigationBar from './navigationBar'
import { AGSContext } from '../helpers/common/agsContext'
import '../styles/theme.css'
import '../styles/common.css'
import { MasterProps } from '../models/components/masterProps'

export function Master({children, agsContext}: MasterProps){
    return (
        <div>
            <AGSContext.Provider value={agsContext} >
                <header>
                    <NavigationBar/>
                </header>
                <main className="container-fluid">
                    {children}
                </main>
            </AGSContext.Provider>
        </div>
    )
}