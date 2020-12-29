import React, { useContext, useState } from 'react';
import { Form, FormGroup, Label, Input, FormText, Badge, Button } from 'reactstrap';
import { InitializePageWithMaster } from '../../helpers/common/masterHelper.js'
import { Master } from '../../components/Master.js'
import { AGSContext } from '../../helpers/common/agsContext.js'
import axios from 'axios'
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'
import { resposne_success } from '../../config/identity.js'


export default function ChangePasswordUIWithMaster({ agsContext, pageProps }) {
    return (
        <Master agsContext={agsContext}>
            <ChangePasswordUI {...pageProps} />
        </Master>
    )
}

function ChangePasswordUI(){
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [newPassword2, setNewPassword2] = useState("");
    const [error, setError] = useState("")
    const [isSaving, setIsSaving] = useState(false);

    const onSaveClick = async () => {
        setIsSaving(true);
        setError("")

        if (newPassword == null || newPassword == ""){
            setError(GetLocalizedString("error_identity_changePassword_password_mandatory"))
            setIsSaving(false);
            return ;
        }

        if (newPassword != newPassword2){
            setError(GetLocalizedString("error_identity_changePassword_not_match_newpassword"))
            setIsSaving(false);
            return ;
        }

        const result = await axios.post("/api/identity/users/changepw", { changePWReuqest: {oldPassword, newPassword }});
        if (result.data.code == resposne_success) {
            alert(GetLocalizedString("label_common_save_succeeded"))
            location.reload();
        } else {
            alert(GetLocalizedString("label_common_save_failed"))
            setError(result.data.code);
        }

        setIsSaving(false);
    }

    return (
        <React.Fragment>
            <Form>
                <FormGroup>
                    <Label>{GetLocalizedString("label_identity_changePassword_oldpassword")}</Label>
                    <Input name="OldPassword" type="password" value={oldPassword} onChange={(e) => setOldPassword(e.target.value)}></Input>
                </FormGroup>
                <FormGroup>
                    <Label>{GetLocalizedString("label_identity_changePassword_newpassword")}</Label>
                    <Input name="NewPassword" type="password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)}></Input>
                </FormGroup>
                <FormGroup>
                    <Label>{GetLocalizedString("label_identity_changePassword_newpassword2")}</Label>
                    <Input name="NewPassword2" type="password" value={newPassword2} onChange={(e) => setNewPassword2(e.target.value)}></Input>
                </FormGroup>
                <Button color="primary" onClick={onSaveClick} disabled={isSaving}>{GetLocalizedString("label_common_button_confirm")}</Button> 
                <Label className="text-danger">{error}</Label>
            </Form>
        </React.Fragment>
    )
}

export async function getServerSideProps(context) {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        return null;
    })

    return result;
}