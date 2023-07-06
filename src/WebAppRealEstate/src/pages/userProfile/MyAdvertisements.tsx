import { Advertisement } from '@/components';
import { UserContext } from '@/contexts';
import { initialAxiosResponse } from '@/helpers';
import { useGetMyAdvertisementsQuery } from '@/services';
import { PublishAdveretismentProps } from '@/types';
import { Box } from '@chakra-ui/react';
import { useContext } from 'react';

export const MyAdvertisements = () => {
  const { currentUser } = useContext(UserContext);

  const {data: myAdvertisementsData = initialAxiosResponse} = useGetMyAdvertisementsQuery(currentUser?.id ?? '');

  return (
    <>
      <Advertisement advertisement={myAdvertisementsData} isHomePage={false} />
    </>
  );
};
