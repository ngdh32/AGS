import { Master } from '../../components/Master.js'
import { AGSContext } from '../../helpers/common/agsContext.js'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper.js'
import { useContext } from 'react'
import GroupsHelper from '../../helpers/Identity/api/groupshelper.js'
import { Table } from 'reactstrap';
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'

export default function GroupUIWithMaster({ agsContext, pageProps }) {
    return (
        <Master agsContext={agsContext}>
            <GroupUI {...pageProps} />
        </Master>
    )
}

function GroupUI({ groups }) {
    const agsContext = useContext(AGSContext);

    return (
        groups == null ?
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
                                {/* <th>Description</th> */}
                            </tr>
                        </thead>
                        <tbody>
                            {
                                groups.map(x => {
                                    return (
                                        <tr>
                                            <td>{x.name}</td>
                                            {/* <td>{x.description}</td> */}
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
        const groupsHelper = new GroupsHelper(context.req, context.res);
        const groups = await groupsHelper.GetGroups();
        
        return {
            groups
        }
    })

    return result;
}