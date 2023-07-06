import { Advertisement } from '@/components';
import { initialAxiosResponse } from '@/helpers';
import { useGetAllAdvertisementsQuery } from '@/services';
import { Box, Spinner } from '@chakra-ui/react';

export const AllAdvertisements = () => {

  const { isLoading, data: allAdvertisements = initialAxiosResponse } = useGetAllAdvertisementsQuery();

  return (
    <>
      {isLoading ? (
        <Spinner />
      ): (
        <Box ml={3}>
          <Advertisement advertisement={allAdvertisements} isMyAdvertisement={true} />
        </Box>
      )}
    </>
  );
};
