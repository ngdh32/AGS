import { Master } from '../../components/Master.js'
import { AGSContext } from '../../helpers/common/agsContext.js'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper.js'
import { useContext } from 'react'
import FunctionClaimsHelper from '../../helpers/Identity/api/functionClaimsHelper.js'
import { Table } from 'reactstrap';
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'


export default function FunctionClaimUIWithMaster({ agsContext, pageProps }) {
    return (
        <Master agsContext={agsContext}>
            <FunctionClaimUI {...pageProps} />
        </Master>
    )
}

function FunctionClaimUI({ functionClaims }) {
    const agsContext = useContext(AGSContext);

    return (
        functionClaims == null ?
            (
                <div>
                    <span>{GetLocalizedString(label_no_data_return)}</span>
                </div>
            ) :
            (
                <div>
                    <Table hover>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                functionClaims.map(x => {
                                    return (
                                        <tr>
                                            <td>{x.name}</td>
                                            <td>{x.description}</td>
                                        </tr>
                                    )
                                })
                            }
                        </tbody>
                    </Table>
                </div>
            )
    )
}

export async function getServerSideProps(context) {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const functionClaimsHelper = new FunctionClaimsHelper(context.req, context.res);
        const functionClaims = await functionClaimsHelper.GetFunctionClaims();
        
        return {
            functionClaims
        }
    })

    return result;
}