import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Form, FormGroup, Label, Input, FormText, Badge } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useContext, useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/editModal.css"
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'

export default function GroupsEditModal({editData, setEditData, functionClaims}){
    const [filererFunctionClaimsValue, setFilererFunctionClaimsValue] = useState("");
    const [filteredFunctionClaims, setFilteredFunctionClaims] = useState(functionClaims);
    const [showFunctionClaimsOptions, setShowFunctionClaimsOptions] = useState(false);

    const onFunctionClaimsInputChange = (input) => {
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

    const onModalClick = (e) => {
        const functionClaimsList = document.getElementById("functionClaimsList")
        if (e.target != functionClaimsList) {
            setShowFunctionClaimsOptions(false)
        }
    }

    const onFunctionClaimsOptionClick = (functionClaimId) => {
        editData.functionClaimIds.push(functionClaimId);
        setEditData({ ...editData })
        setShowFunctionClaimsOptions(false)
        setFilererFunctionClaimsValue('')
    }

    const onFunctionClaimBadgeRemoveClick = (functionClaimId) => {
        const updatedFunctionClaimIds = []; 
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


    const onValueChange = (e) => {
        editData[e.target.getAttribute("name")] = e.target.value;
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
                        <Input type="text" name="name" placeholder={GetLocalizedString("label_identity_group_table_name")} value={editData.name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_group_table_functionClaim")}:
                        </Label>
                        <div class="form-control autocomplete">
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

function FunctionClaimBadge({functionClaim, onFunctionClaimBadgeRemoveClick}) {
    return (
        <React.Fragment>
            <Button className="optionBadge" color="secondary" type="button">{functionClaim.name} <FontAwesomeIcon icon={faTimes} onClick={() => onFunctionClaimBadgeRemoveClick(functionClaim.id)}></FontAwesomeIcon></Button>
        </React.Fragment>
    )
}