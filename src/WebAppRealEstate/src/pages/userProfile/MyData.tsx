import { useTranslation } from 'react-i18next';
import { AxiosResponse } from 'axios';
import { Box, Button, Text as Info, VStack } from '@chakra-ui/react';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

import { CustomInput } from '@/components';
import { useErrorToast, useSuccessToast } from '@/helpers';
import { useEditUserMutation } from '@/services';
import { useQueryClient } from '@tanstack/react-query';
import { UserDetailsType } from '@/types';
import { useContext, useEffect } from 'react';
import { UserContext } from '@/contexts/UserProvider';

export const MyData = () => {
  const [t] = useTranslation('common');
  const successToast = useSuccessToast();
  const errorToast = useErrorToast();
  const queryClient = useQueryClient();
  const { currentUser } = useContext(UserContext);
  console.log(currentUser);

  const { mutate: editUser } = useEditUserMutation(queryClient, {
    onSuccess: (response?: AxiosResponse) => {
      successToast({ title: t('successfulEditUser', { response }) });
    },
    onError: () => {
      errorToast({ title: t('unsuccessfulEditUser') });
    }
  });

  const initialValues = {
    firstName: currentUser?.firstName || '',
    lastName: currentUser?.lastName || '',
    address: currentUser?.address || '',
    phoneNumber: currentUser?.phoneNumber || ''
  };

  const userProfileSchema = Yup.object().shape({
    firstName: Yup.string(),
    lastName: Yup.string(),
    address: Yup.string(),
    phoneNumber: Yup.string(),
  });

  const handleSubmit = (values: UserDetailsType) => {
    const payload = {
      firstName: values?.firstName,
      lastName: values?.lastName,
      address: values?.address,
      phoneNumber: values?.phoneNumber
    };
    console.log(payload);
    editUser({id: currentUser?.id ?? '', payload: payload});
  };

  return (
    <>
      <VStack mt='2%'>
        <Box boxShadow='2xl' bg='blue.200' borderRadius='10' padding='5%' textColor='white'>
          <Formik
            validateOnMount
            validateOnChange
            initialValues={initialValues}
            validationSchema={userProfileSchema}
            onSubmit={handleSubmit}>
            {({ isSubmitting, isValid, errors, touched }) => (
              <Form>
                <CustomInput
                  type='text'
                  maxLength={50}
                  isInvalid={!!errors.firstName && touched.firstName}
                  name='firstName'
                  label={t('firstName')}
                  error={errors.firstName}
                />
                <CustomInput
                  type='text'
                  maxLength={50}
                  isInvalid={!!errors.lastName && touched.lastName}
                  name='lastName'
                  label={t('lastName')}
                  error={errors.lastName}
                />
                <CustomInput
                  type='text'
                  maxLength={50}
                  isInvalid={!!errors.address && touched.address}
                  name='address'
                  label={t('address')}
                  error={errors.address}
                />
                <CustomInput
                  type='text'
                  maxLength={50}
                  isInvalid={!!errors.phoneNumber && touched.phoneNumber}
                  name='phoneNumber'
                  label={t('phoneNumber')}
                  error={errors.phoneNumber}
                />
                <Info fontWeight='500'>{t('points')}</Info>
                <Info>{`${currentUser?.points}`}</Info>
                <Button
                  type='submit'
                  top='15px'
                  minW='100px'
                  size='lg'
                  bg='blue.600'
                  _hover={{bg: 'blue.400'}}
                  ml='65%'
                  cursor='pointer'
                  disabled={isSubmitting || !isValid}>
                  {t('edit')}
                </Button>
              </Form>
            )}
          </Formik>
        </Box>
      </VStack>
    </>
  );
};
