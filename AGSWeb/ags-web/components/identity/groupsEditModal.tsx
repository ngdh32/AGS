import { Button, ModalBody } from 'reactstrap';
import { Form, FormGroup, Label, Input } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/editModal.css"
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import { GroupEditModalProps } from '../../models/components/identity/groupEditModalProps';
import { FunctionClaimBadgeProps } from '../../models/components/identity/functionClaimBadgeProps';
import { GroupItemType } from '../../models/identity/groupItemType';
import { InputEventProps } from '../../models/common/inputEventProps';

export default function GroupsEditModal({editData, setEditData, functionClaims} : GroupEditModalProps){
    const [filererFunctionClaimsValue, setFilererFunctionClaimsValue] = useState("");
    const [filteredFunctionClaims, setFilteredFunctionClaims] = useState(functionClaims);
    const [showFunctionClaimsOptions, setShowFunctionClaimsOptions] = useState(false);

    const onFunctionClaimsInputChange = (input: string) => {
        setFilererFunctionClaimsValue(input);

        let functionClaimsOptions = input == null || input == "" ? functionClaims : functionClaims.filter(x => {
            return x.name.toUpperCase().includes(input.toUpperCase());
        });

        functionClaimsOptions = functionClaimsOptions.filter(x => {
            const isAdded = editData.functionClaimIds.some(y => {
                return y == x.id
            })
            return isAdded ? false : true;
        })

        setFilteredFunctionClaims(functionClaimsOptions);
        setTimeout(() => setShowFunctionClaimsOptions(true), 0);
    }

    const onModalClick = (e: React.MouseEvent<HTMLElement, MouseEvent>) => {
        const functionClaimsList = document.getElementById("functionClaimsList")
        if (e.target != functionClaimsList) {
            setShowFunctionClaimsOptions(false)
        }
    }

    const onFunctionClaimsOptionClick = (functionClaimId: string) => {
        editData.functionClaimIds.push(functionClaimId);
        setEditData({ ...editData })
        setShowFunctionClaimsOptions(false)
        setFilererFunctionClaimsValue('')
    }

    const onFunctionClaimBadgeRemoveClick = (functionClaimId: string) => {
        const updatedFunctionClaimIds : string[] = []; 
        editData.functionClaimIds.forEach(x => {
            if (x != functionClaimId){
                updatedFunctionClaimIds.push(x);
            }
        })
        console.log(updatedFunctionClaimIds)
        
        editData.functionClaimIds = updatedFunctionClaimIds;
        setEditData({...editData})
    }

    const selectedFunctionClaimBadges = editData.functionClaimIds == null ? (<React.Fragment></React.Fragment>) : editData.functionClaimIds.map(x => {
        const functionClaim = functionClaims.find(y => {return y.id == x});
        return (
            <FunctionClaimBadge functionClaim={functionClaim} onFunctionClaimBadgeRemoveClick={onFunctionClaimBadgeRemoveClick}/>
        )
    })

    const onNameChange = (e: InputEventProps) => {
        editData.name = e.target.value;
        setEditData({ ...editData })
    }



    return (
        <React.Fragment>
            <ModalBody onClick={onModalClick}>
                <Form>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_group_table_name")}:
                        </Label>
                        <Input type="text" name="name" placeholder={GetLocalizedString("label_identity_group_table_name")} value={editData.name} onChange={(e) => onNameChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_group_table_functionClaim")}:
                        </Label>
                        <div className="form-control autocomplete">
                            {
                                selectedFunctionClaimBadges
                            }
                            <div>
                                <Input className="input" type="text" 
                                    onClick={() => onFunctionClaimsInputChange(null)} 
                                    onChange={(e) => { onFunctionClaimsInputChange(e.target.value) }}
                                    value={filererFunctionClaimsValue}
                                    >    
                                </Input>
                                <div id="groupList">
                                    <ListGroup className={"optionList " + (showFunctionClaimsOptions ? "" : "hide")}>
                                        {
                                            filteredFunctionClaims.map(x => {
                                                return (
                                                    <ListGroupItem className="option" action onClick={() => onFunctionClaimsOptionClick(x.id)}>
                                                        {x.name}
                                                    </ListGroupItem>
                                                )
                                            })
                                        }
                                    </ListGroup>
                                </div>
                            </div>
                        </div>
                    </FormGroup>
                </Form>
            </ModalBody>
        </React.Fragment>
    )
}

function FunctionClaimBadge({functionClaim, onFunctionClaimBadgeRemoveClick}: FunctionClaimBadgeProps) {
    return (
        <React.Fragment>
            <Button className="optionBadge" color="primary" type="button">{functionClaim.name} <FontAwesomeIcon icon={faTimes} onClick={() => onFunctionClaimBadgeRemoveClick(functionClaim.id)}></FontAwesomeIcon></Button>
        </React.Fragment>
    )
}